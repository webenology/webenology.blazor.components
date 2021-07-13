using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

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
        private string yes = "Yes";
        private string no = "No";
        private string cancel = "Cancel";
        private bool _isSaving;
        private bool _showCancel = true;
        private bool _showNo = true;

        private async Task Execute(Action action)
        {
            _isSaving = true;
            await Task.Delay(10);
            _isOpen = false;
            action?.Invoke();
            StateHasChanged();
        }

        public void ShowConfirm(string confirmHeader, string confirmMessage, Action OnYes = null, Action OnNo = null, Action OnCancel = null, string yesName = null, string noName = null, string cancelName = null, bool showCancel = true, bool showNo = true)
        {
            _header = confirmHeader;
            _content = confirmMessage;
            _isOpen = true;
            _onYes = OnYes;
            _onNo = OnNo;
            _onCancel = OnCancel;
            cancel = cancelName ?? cancel;
            yes = yesName ?? yes;
            no = noName ?? no;
            _isSaving = false;
            _showCancel = showCancel;
            _showNo = showNo;

            StateHasChanged();
        }

    }
}
