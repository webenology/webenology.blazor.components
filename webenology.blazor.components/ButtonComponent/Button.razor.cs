using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components.ButtonComponent
{
    public partial class Button
    {
        [Parameter]
        public string Text { get; set; }
        [Parameter]
        public ButtonType ViewType { get; set; }
        [Parameter]
        public EventCallback OnClick { get; set; }
        [Parameter]
        public RenderFragment ChildContent { get; set; }
        [Parameter]
        public bool IsSmall { get; set; }
        [Parameter]
        public string ButtonPrimaryCss { get; set; }
        [Parameter]
        public string ButtonSecondaryCss { get; set; }
        [Parameter]
        public string ButtonSuccessCss { get; set; }
        [Parameter]
        public string ButtonDangerCss { get; set; }
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }


        private string _bgType = "btn";

        protected override void OnParametersSet()
        {
            var buttonCss = new List<string> { "btn" };

            if (ViewType == ButtonType.Primary)
                buttonCss.Add(ButtonPrimaryCss ?? "btn-primary");
            else if (ViewType == ButtonType.Secondary)
                buttonCss.Add(ButtonSecondaryCss ?? "btn-primary2");
            else if (ViewType == ButtonType.Success)
                buttonCss.Add(ButtonSuccessCss ?? "btn-success");
            else if (ViewType == ButtonType.Danger)
                buttonCss.Add(ButtonDangerCss ?? "btn-danger");

            if (IsSmall)
                buttonCss.Add("btn-sm");

            _bgType = string.Join(" ", buttonCss);

            base.OnParametersSet();
        }

        public enum ButtonType
        {
            Primary,
            Secondary,
            Success,
            Danger
        }
    }
}
