var DateTimePicker = /** @class */ (function () {
    function DateTimePicker() {
    }
    DateTimePicker.setupPicker = function (instance, element, type, time, makeStatic) {
        flatpickr(element, {
            dateFormat: time ? "m-d-Y h:i:s K" : "m-d-Y",
            weekNumbers: true,
            enableTime: time,
            static: makeStatic,
            mode: type,
            onChange: function (e) {
                var noTimeZone = [];
                for (var i = 0; i < e.length; i++) {
                    var offset = e[i].getTimezoneOffset() * 60000;
                    var dateinUtc = e[i].getTime();
                    noTimeZone.push(new Date(dateinUtc - offset));
                }
                instance.invokeMethodAsync("OnChange", noTimeZone);
            }
        });
    };
    DateTimePicker.updateSetting = function (element, setting, value) {
        var el = flatpickr(element);
        el.set(setting, value);
    };
    return DateTimePicker;
}());
//# sourceMappingURL=DateTimePicker.js.map