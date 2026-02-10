export function PositionCalendar(el) {
    var spanBoundingBox = el.getBoundingClientRect();
    var input = el.querySelector(".wc-holder");
    var inputBoundingBox = input.getBoundingClientRect();

    var screenWidth = window.innerWidth;
    var screenHeight = window.innerHeight;

    var endingRight = screenWidth - (spanBoundingBox.left + inputBoundingBox.width);
    var endingBottom = screenHeight - (spanBoundingBox.bottom + inputBoundingBox.height);
    endingRight = Math.max(20, endingRight);
    endingBottom = Math.max(20, endingBottom);
    input.style.right = `${endingRight}px`;
    input.style.bottom = `${endingBottom}px`;

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