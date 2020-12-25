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
        public ButtonStyle Style { get; set; } = ButtonStyle.WebenologyStyle;
        [Parameter(CaptureUnmatchedValues = true)]
        public Dictionary<string, object> Attributes { get; set; }

        private string GetCss()
        {
            var buttonCss = new List<string> { Style.ButtonCss };

            if (ViewType == ButtonType.Primary)
                buttonCss.Add(Style.ButtonPrimaryCss);
            else if (ViewType == ButtonType.Secondary)
                buttonCss.Add(Style.ButtonSecondaryCss);
            else if (ViewType == ButtonType.Success)
                buttonCss.Add(Style.ButtonSuccessCss);
            else if (ViewType == ButtonType.Danger)
                buttonCss.Add(Style.ButtonDangerCss);

            if (IsSmall)
                buttonCss.Add(Style.SmallCss);

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
