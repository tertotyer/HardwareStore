new Swiper('.image-slider', {

    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev'
    },
    loop: true,
    effect: 'fade',
    fadeEffect:{
        crossFade:true
    },

    //Превью
    thumbs: {
        swiper: {
            el: '.image-mini-slider',
            slidesPerView: 5,
            spaceBetween: 30,
        }
    }
});