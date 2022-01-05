declare const flatpickr: any;


export function setupPicker(instance, element, type, time, makeStatic, isInline, minDate, maxDate) {
    var config = {
        dateFormat: time ? "m-d-Y G:i:S K" : "m-d-Y",
        weekNumbers: true,
        enableTime: time,
        static: makeStatic,
        inline: isInline,
        minDate: minDate,
        maxDate: maxDate,
        mode: type,
        onChange: (e) => {
            const noTimeZone = [];
            for (let i = 0; i < e.length; i++) {
                const offset = e[i].getTimezoneOffset() * 60000;
                const dateinUtc = e[i].getTime();
                noTimeZone.push(new Date(dateinUtc - offset));
            }
            instance.invokeMethodAsync("OnChange", noTimeZone);
        }
    };

    flatpickr(element, config);
}

export function updateSetting(element, setting, value) {
    const el = element._flatpickr;
    el.set(setting, value);

}

export function openCalendar(element) {
    const el = element._flatpickr;
    el.open();
}
