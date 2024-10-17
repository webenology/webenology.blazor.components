namespace webenology.blazor.components.ConfirmComponent
{
    public class ConfirmStyle
    {

        public static ConfirmStyle WebenologyStyle => new ConfirmStyle
        {
            BackdropCss = "backdrop !bg-black/80",
            HideCss = "hidden",
            BoundInCss = "bounce-in",
            CardCss = "dark:!bg-blackdp1 p-2 flex flex-col gap-2",
            CardHeaderCss = "text-2xl font-semibold",
            CardBodyCss = "card-body",
            CardFooterCss = "card-footer text-right",
            CancelButtonCss = "modal-button-close",
            NoButtonCss = "button-danger",
            YesButtonCss = "button-primary"
        };

        public string BackdropCss { get; set; }
        public string HideCss { get; set; }
        public string BoundInCss { get; set; }
        public string CardCss { get; set; }
        public string CardHeaderCss { get; set; }
        public string CardBodyCss { get; set; }
        public string CardFooterCss { get; set; }
        public string CancelButtonCss { get; set; }
        public string NoButtonCss { get; set; }
        public string YesButtonCss { get; set; }
    }
}
