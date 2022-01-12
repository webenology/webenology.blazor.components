export function setupPicker(instance, element, type, time, makeStatic, isInline, minDate, maxDate, timeOnly) {
    var config = {
        dateFormat: time ? "m-d-Y G:i:S K" : "m-d-Y",
        weekNumbers: true,
        enableTime: time,
        static: makeStatic,
        inline: isInline,
        minDate: minDate,
        maxDate: maxDate,
        mode: type,
        onChange: function (e) {
            var noTimeZone = [];
            for (var i = 0; i < e.length; i++) {
                var offset = e[i].getTimezoneOffset() * 60000;
                var dateInUtc = e[i].getTime();
                noTimeZone.push(new Date(dateInUtc - offset));
            }
            instance.invokeMethodAsync("OnChange", noTimeZone);
        }
    };
    if (timeOnly) {
        config["enableTime"] = true;
        config["noCalendar"] = true;
        config["dateFormat"] = "G:i K";
    }
    flatpickr(element, config);
}
export function updateSetting(element, setting, value) {
    var el = element._flatpickr;
    el.set(setting, value);
}
export function openCalendar(element) {
    var el = element._flatpickr;
    el.open();
}
