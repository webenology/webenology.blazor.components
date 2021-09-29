using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;

namespace webenology.blazor.components
{
    public partial class ComboBox<TItem> : IDisposable
    {
        [Parameter]
        public string Label { get; set; }
        [Parameter]
        public List<TItem> Items { get; set; }
        [Parameter]
        public string ValueFieldName { get; set; }
        [Parameter]
        public bool CanAddNewItem { get; set; }
        [Parameter]
        public EventCallback<string> OnCreateNewItem { get; set; }
        [Parameter]
        public EventCallback<TItem> OnSelectedItem { get; set; }
        [Parameter]
        public TItem SelectedItem { get; set; }
        [Parameter]
        public ComboBoxType ComboBoxType { get; set; }
        [Parameter]
        public bool IsEditable { get; set; }
        [Parameter]
        public string NewTypeName { get; set; }
        [Parameter]
        public bool ShowRemoveButton { get; set; }
        [Parameter]
        public int MaxItemsToShow { get; set; }
        [Parameter]
        public Expression<Func<object>> For { get; set; }
        [Parameter]
        public string PlaceHolder { get; set; }
        [Parameter]
        public ComboBoxStyle CssStyle { get; set; } = ComboBoxStyle.WebenologyStyle;
        [Parameter]
        public bool Readonly { get; set; }

        [Parameter] public int ItemHeight { get; set; } = 40;
        [Parameter] public EventCallback<string> ValueChanged { get; set; }
        [Parameter] public bool AllowFreeFormText { get; set; }
        [CascadingParameter]
        private EditContext _editContext { get; set; }

        [Inject]
        private IComboBoxJsHelper jsHelper { get; set; }

        private string _localText;
        private string LocalText
        {
            get => _localText;
            set
            {
                _areItemsOpen = true;
                _currentFocused = -1;
                _localText = value;
                _virtualized.RefreshDataAsync();
            }
        }
        private bool _areItemsOpen;
        private bool _isError => !string.IsNullOrEmpty(_errorMessage);
        private string _errorMessage;
        private Dictionary<string, object> _attributes = new();
        private ElementReference _elRef;
        private ElementReference _scrollEl;
        private Virtualize<TItem> _virtualized;
        private int _currentFocused = -1;

        private List<TItem> SearchedItems
        {
            get
            {
                if (string.IsNullOrEmpty(_localText))
                    return Items;

                return Items.Where(x => ContainsIn(x, _localText)).ToList();
            }
        }

        private void closeItemsWindow()
        {
            if (!EqualityComparer<TItem>.Default.Equals(SelectedItem, default))
                _localText = GetValue(SelectedItem);

            _areItemsOpen = false;
        }

        private void openItemsWindow()
        {
            if (Readonly)
                return;

            _virtualized.RefreshDataAsync();

            _areItemsOpen = true;
        }

        private void onSelectItem(TItem item)
        {
            if (EqualityComparer<TItem>.Default.Equals(item, default) && AllowFreeFormText)
            {
                SelectedItem = default;
                OnSelectedItem.InvokeAsync(default);
                ValueChanged.InvokeAsync(_localText);
            }
            else
            {
                ValueChanged.InvokeAsync(string.Empty);
                _localText = GetValue(item);
                OnSelectedItem.InvokeAsync(item);
                SelectedItem = item;
            }
            _currentFocused = -1;
            closeItemsWindow();
        }

        private void toggleItemsWindows()
        {
            if (Readonly)
                return;

            _elRef.FocusAsync();
            _areItemsOpen = !_areItemsOpen;
        }

        private void onAddNewItem()
        {
            OnCreateNewItem.InvokeAsync(_localText);
        }

        private void clearItem()
        {
            _localText = string.Empty;
            onSelectItem(default);
        }

        private void onKeyPress(KeyboardEventArgs args)
        {
            if (args.Code == "ArrowUp")
            {
                if (!_areItemsOpen)
                    _areItemsOpen = true;

                if (_currentFocused <= 0)
                    _currentFocused = SearchedItems.Count - 1;
                else
                    _currentFocused--;

                jsHelper.ScrollTo(_scrollEl, _currentFocused);
            }
            else if (args.Code == "ArrowDown")
            {
                if (!_areItemsOpen)
                    _areItemsOpen = true;

                if (_currentFocused >= SearchedItems.Count - 1)
                    _currentFocused = 0;
                else
                    _currentFocused++;

                jsHelper.ScrollTo(_scrollEl, _currentFocused);
            }
            else if (args.Code == "Enter" && _currentFocused > -1)
            {
                onSelectItem(SearchedItems[_currentFocused]);
            }
            else if (args.CtrlKey && args.Code == "Enter")
            {
                if (CanAddNewItem)
                    OnCreateNewItem.InvokeAsync(_localText);
            }
            else if (args.Code == "Enter")
            {
                OnEnter();
            }
        }

        private void OnEnter()
        {
            var item = SearchedItems.FirstOrDefault(x =>
                GetValue(x).Equals(_localText, StringComparison.OrdinalIgnoreCase));
            if (EqualityComparer<TItem>.Default.Equals(item, default))
                onSelectItem(default);
            else
                onSelectItem(item);
        }

        private ValueTask<ItemsProviderResult<TItem>> GetSearchedItems(
            ItemsProviderRequest request)
        {
            var numSearched = Math.Min(request.Count, SearchedItems.Count - request.StartIndex);
            var searched = SearchedItems.Skip(request.StartIndex).Take(numSearched);

            return ValueTask.FromResult(new ItemsProviderResult<TItem>(searched, SearchedItems.Count));
        }

        private string GetValue(TItem item)
        {
            if (item == null)
                return string.Empty;

            if (typeof(TItem) == typeof(string))
                return item.ToString();

            if (string.IsNullOrEmpty(ValueFieldName))
                throw new ArgumentNullException($"ValueFieldName is a required field!");

            var property = item.GetType().GetProperty(ValueFieldName);
            if (property == null)
                throw new ArgumentNullException($"Value field name: {ValueFieldName} does not exist on object");

            var propertyValue = property.GetValue(item);

            if (propertyValue == null)
                return string.Empty;

            return property.GetValue(item)?.ToString();
        }

        private bool ContainsIn(TItem item, string search)
        {
            var val = GetValue(item);
            return val.Contains(search, StringComparison.OrdinalIgnoreCase);
        }

        private bool ExactMatch()
        {
            return SearchedItems.Any(x => GetValue(x).Equals(_localText, StringComparison.OrdinalIgnoreCase));
        }

        protected override void OnInitialized()
        {
            if (!IsEditable)
                _attributes.Add("readonly", "readonly");

            if (_editContext != null)
            {
                _editContext.OnValidationRequested += _editContext_OnValidationRequested;
            }

            base.OnInitialized();
        }

        private void _editContext_OnValidationRequested(object sender, ValidationRequestedEventArgs e)
        {
            _errorMessage = string.Empty;
            if (_editContext != null && For != null)
            {
                var fieldIdentifier = new FieldIdentifier(_editContext.Model, ((MemberExpression)For.Body).Member.Name);
                var message = _editContext.GetValidationMessages(fieldIdentifier).ToList();
                if (message.Any())
                {
                    _errorMessage = message.First();
                }
            }
        }

        private string InputCss()
        {
            var css = new List<string> { CssStyle.InputCss };

            if (ShowRemoveButton)
                css.Add(CssStyle.InputShowRemoveInputCss);

            return string.Join(" ", css);
        }

        private string ListGroupCss()
        {
            var css = new List<string> { CssStyle.ListGroupCss };

            if (!_areItemsOpen)
                css.Add(CssStyle.ListGroupHideCss);

            return string.Join(" ", css);
        }
        private string ListGroupItemCss(bool isFocused)
        {
            var css = new List<string> { CssStyle.ListGroupItemCss };

            if (isFocused)
                css.Add(CssStyle.ListGroupItemFocusedCss);

            return string.Join(" ", css);
        }


        protected override void OnParametersSet()
        {
            if (!EqualityComparer<TItem>.Default.Equals(SelectedItem, default))
            {
                var val = GetValue(SelectedItem);
                _localText = val;
            }

            base.OnParametersSet();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                jsHelper.StopArrows(_elRef);
            }
            base.OnAfterRender(firstRender);
        }

        public void Dispose()
        {
            if (_editContext != null)
            {
                _editContext.OnValidationRequested -= _editContext_OnValidationRequested;
            }
        }

    }
}
