declare const flatpickr: any;

export function setupPicker(instance, element, type, time, makeStatic) {
    flatpickr(element, {
        dateFormat: time ? "m-d-Y G:i:S K" : "m-d-Y",
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
}

export function updateSetting(element, setting, value) {

    const el = flatpickr(element);
    el.set(setting, value);
}
