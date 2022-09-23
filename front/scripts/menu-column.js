$(document).ready(function(){
    $('.category-menu__item').click(function(event) {
        $(this).toggleClass('category-menu__item--opened').next().slideToggle(300);
    });
});