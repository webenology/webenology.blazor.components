declare const flatpickr: any;


export function setupPicker(instance, element, type, time, makeStatic, isInline, minDate, maxDate, timeOnly) {
    const config = {
        dateFormat: time ? "m-d-Y G:i:S K" : "m-d-Y",
        weekNumbers: true,
        enableTime: time,
        static: makeStatic,
        inline: isInline,
        minDate: minDate,
        maxDate: maxDate,
        mode: type,
        onOpen: (e) => {
            instance.invokeMethodAsync("CanOpen").then(canOpen => {
                console.log(canOpen);
                if (!canOpen) {
                    closeCalendar(element);
                }
            });
        },
        onChange: (e) => {
            const noTimeZone = [];
            for (let i = 0; i < e.length; i++) {
                const offset = e[i].getTimezoneOffset() * 60000;
                const dateInUtc = e[i].getTime();
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
    const el = element._flatpickr;
    el.set(setting, value);

}

export function openCalendar(element) {
    const el = element._flatpickr;
    el.open();
}


export function closeCalendar(element) {
    const el = element._flatpickr;
    el.close();
}
