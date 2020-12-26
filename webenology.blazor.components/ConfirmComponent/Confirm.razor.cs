using System;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
{
    public partial class Confirm
    {
        [Parameter]
        public ConfirmStyle CssStyle { get; set; } = ConfirmStyle.WebenologyStyle;
        private string _header;
        private string _content;
        private bool _isOpen;
        private Action _onYes;
        private Action _onNo;
        private Action _onCancel;

        private void Execute(Action action)
        {
            _isOpen = false;
            action?.Invoke();
        }

        public void ShowConfirm(string confirmHeader, string confirmMessage, Action OnYes = null, Action OnNo = null, Action OnCancel = null)
        {
            _header = confirmHeader;
            _content = confirmMessage;
            _isOpen = true;
            _onYes = OnYes;
            _onNo = OnNo;
            _onCancel = OnCancel;
            StateHasChanged();
        }
    }
}
