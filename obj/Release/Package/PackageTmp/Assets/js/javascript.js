$(document).ready(function(){
    $('.owl-carousel').owlCarousel({
        loop:true,
        margin:0,
        nav:true,
        autoplay: 5000,
        responsive:{
            0:{
                items:1
            },
            600:{
                items:1
            },
            890:{
                items:1
            }
        }
    })


    $("#show1").click(function () {
        
        $("#map1").addClass("bubble");
        $("#map2").removeClass("bubble");
    })

    $("#show2").click(function(){
        $("#map2").addClass("bubble");
        $("#map1").removeClass("bubble");
    })


});


