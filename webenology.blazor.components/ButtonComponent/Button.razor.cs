using System.Collections.Generic;

using Microsoft.AspNetCore.Components;

namespace webenology.blazor.components
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
        public ButtonStyle CssStyle { get; set; } = ButtonStyle.WebenologyStyle;
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        private string GetCss()
        {
            var buttonCss = new List<string> { CssStyle.ButtonCss };

            if (ViewType == ButtonType.Primary)
                buttonCss.Add(CssStyle.ButtonPrimaryCss);
            else if (ViewType == ButtonType.Secondary)
                buttonCss.Add(CssStyle.ButtonSecondaryCss);
            else if (ViewType == ButtonType.Success)
                buttonCss.Add(CssStyle.ButtonSuccessCss);
            else if (ViewType == ButtonType.Danger)
                buttonCss.Add(CssStyle.ButtonDangerCss);

            if (IsSmall)
                buttonCss.Add(CssStyle.SmallCss);

            return string.Join(" ", buttonCss);

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
