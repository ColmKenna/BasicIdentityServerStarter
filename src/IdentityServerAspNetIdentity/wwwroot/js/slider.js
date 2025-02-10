document.querySelectorAll('.slider-container').forEach(container => {
    const slider = container.querySelector('input[type="range"]');
    const numberInput = container.querySelector('input[type="number"]');

    slider.addEventListener('input', () => {
        numberInput.value = slider.value;
    });

    numberInput.addEventListener('input', () => {
        slider.value = numberInput.value;
    });
});