export function PositionCalendar(el) {
    var spanBoundingBox = el.getBoundingClientRect();
    var input = el.querySelector(".wc-holder");
    var inputBoundingBox = input.getBoundingClientRect();

    var screenWidth = window.innerWidth;

    console.log(inputBoundingBox.right + 25, screenWidth);

    if (inputBoundingBox.right + 25 > screenWidth) {
        var right = spanBoundingBox.width + 20 + spanBoundingBox.left - window.innerWidth;
        input.style.right = `${right}px`;
    } else {
        input.style.right = null;
    }
}

export function StopPropagation(el, t) {
    el.addEventListener("keydown", (e) => {
        if (e.key == "Enter") {
            e.preventDefault();
            t.invokeMethodAsync("OnEnterHit", el.value);
            return;
        }
    })
}

export function SelectAll(el) {
    el.select();
}