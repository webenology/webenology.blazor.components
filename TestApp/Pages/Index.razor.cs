using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Microsoft.Win32.SafeHandles;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using TestApp.Data;
using webenology.blazor.components;
using webenology.blazor.components.BlazorPdfComponent;
using webenology.blazor.components.MailMerge;
using NavigationException = PuppeteerSharp.NavigationException;

namespace TestApp.Pages
{
    public partial class Index
    {
        private Notification _notification;
        private Modal _modal;
        private Modal _modal2;
        private Confirm _confirm;
        private bool _insideClick;
        private DateTime? _dt = DateTime.Today.AddDays(-3);
        private string _text;
        private int? _num;
        private int _count = 0;
        private List<TreeNode> _nodes = new();
        private string _myvalue;
        private string _myhightlightvalue;
        private string _checkedValue = "All";
        private List<OrderByClass> _orderByClasses = new();
        [Inject] private IJSRuntime js { get; set; }
        [Inject] private IBlazorPdf blazorPdf { get; set; }
        [Inject] private IMailMergeManager mailMergeManager { get; set; }
        [Inject] private NavigationManager nav { get; set; }

        private ConfirmStyle confirmStyle()
        {
            var c = ConfirmStyle.WebenologyStyle;
            c.BackdropCss = "none";
            return c;
        }

        private List<KeyValuePair<string, string>> items = new();
        private KeyValuePair<string, string> _selectedItem;

        private List<DateTime?> _dates = new();
        private string _pdfPreview;
        private bool _isDblClickReadonly = true;

        private Dictionary<string, string> _metabaseData = new()
        {
            { "StartDate", "2023-08-01" },
            { "EndDate", "2023-08-31" },
            { "ClientId", null },
            { "VendorId", null }
        };

        private List<DateTime?> Dates
        {
            get => _dates;
            set => _dates = value;
        }

        private void OnDblClick()
        {
            _isDblClickReadonly = false;
            js.InvokeVoidAsync("alert", "double click");
        }

        private void AddNewItem(string s)
        {
            var itm = new KeyValuePair<string, string>(s, s);
            items.Add(itm);
            _selectedItem = itm;
            StateHasChanged();
        }

        private void AddNotification()
        {
            var success = new NotificationModel { Body = $"Count: {_count}", ShowTimeoutBar = true };

            _notification.AddNotification(success);

            _count++;

            var warning = new NotificationModel
                { Body = $"Count: {_count}", ShowTimeoutBar = true, Type = NotificationType.Warning };

            _notification.AddNotification(warning);

            _count++;

            var danger = new NotificationModel
            {
                Body = $"Count: {_count}", Header = "what what what", ShowTimeoutBar = true,
                Type = NotificationType.Danger, TimeoutInSeconds = 5
            };

            _notification.AddNotification(danger);

            _count++;
        }

        private void onSelected(List<string> claims)
        {
            var c = claims;
        }

        protected override void OnInitialized()
        {
            for (var i = 0; i < 100; i++)
            {
                items.Add(new KeyValuePair<string, string>(i.ToString(), i.ToString()));
            }

            items.Add(new KeyValuePair<string, string>("abc",
                "ajsdl;fjas lkdfj asl;k dfalsk; fdl;aksjfl;kas jflk;asj dfl;aj slkfasldf alks flkasd jfdlkasj fl;asj flkasjdflkasjd fl;ka jl;jas;dlfj asl;kdfj al;skdjf l;as djfklasj df;lkajs dfl;ajsdfl;kjasdl;fhajskdhfjkah lka;sjdflk; jl;asdjfla sdl;fkj asd;lfjoi;wuofhalsdkfjal;ksdjfa ;lsdfjl;kasjdfl;kajs df;lj"));

            var homeNode = new TreeNode("1-abi", "main page");
            homeNode.Nodes.Add(new TreeNode("second node",
                "another node that you need to look at,this one does stuff"));
            homeNode.Nodes.Add(new TreeNode { Node = "third node", IsDisabled = true });
            var fourthNode = new TreeNode("fourth node");
            fourthNode.Nodes.Add(new TreeNode("fourth + 1 node"));
            homeNode.Nodes.Add(fourthNode);

            _nodes.Add(homeNode);
            _nodes.Add(new TreeNode { Node = "Sehab", IsDisabled = true });
            _orderByClasses.Add(new OrderByClass
            {
                Name = "Sehab",
                Age = 2
            });
            _orderByClasses.Add(new OrderByClass
            {
                Name = "Iris",
                Age = 13
            });
            base.OnInitialized();
        }


        private void UpdateDates(List<DateTime> obj)
        {
            // _dates = obj;
        }

        private void UpdateOrderBy(List<OrderByClass> obj)
        {
            _orderByClasses = obj;
        }

        private async Task GeneratePdf()
        {
            var css = new List<string>
            {
                "/css/bootstrap/bootstrap.min.css"
            };
            var baseUrl = "https://localhost:44348";
            var pdfOptions = new PdfOptions
            {
                PrintBackground = true,
                HeaderTemplate = "abc",
                FooterTemplate = "pageNumber of totalPages",
                Landscape = true,
            };
            _pdfPreview =
                await blazorPdf.GetBlazorInPdfBase64<Counter>(x => { }, "abc", css, null, pdfOptions, baseUrl, true);
        }

        private async Task GeneratePdfFromDocx()
        {
            var doc = $"{nav.BaseUri}simpletesttwo.docx";
            await js.InvokeVoidAsync("console.log", doc);
            var http = new HttpClient();
            var fs = await http.GetStreamAsync(doc);
            var obj = new { FirstName = "Jackie", LastName = "Chan" };
            _pdfPreview = mailMergeManager.Merge(fs, obj, true);
        }
    }
}