new Swiper('.image-slider', {

    navigation: {
        nextEl: '.swiper-button-next',
        prevEl: '.swiper-button-prev'
    },
    loop: false,

     //Брейкпоинты
    breakpoints: {
        768:{
            spaceBetween: 15,
            loop: true,
            effect: 'fade',
            fadeEffect:{
                crossFade:true
            },
        },
        1320:{
            spaceBetween: 20,
        }
    },

    //Скроллбар
    scrollbar:{
        el: '.swiper-scrollbar',
        draggable: true,
    },

    //Превью
    thumbs: {
        swiper: {
            el: '.image-mini-slider',
            slidesPerView: 5,
            spaceBetween: 15,
                //Брейкпоинты
            breakpoints: {
                768:{
                    spaceBetween: 15,
                },
                1320:{
                    spaceBetween: 20,
                }
            }
        }
    }
});