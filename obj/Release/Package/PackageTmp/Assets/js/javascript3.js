$(document).ready(function () {
    $("#star1").click(function () {
        $("#numb_star").val(1);
        for (var j = 1; j <= 1; j++) {
            $("#star" + j.toString()).addClass("star-color");
        }
        for (var j = 2; j <= 5; j++) {
            $("#star" + j.toString()).removeClass("star-color");
        }
    });

    $("#star2").click(function () {
        $("#numb_star").val(2);
        for (var j = 1; j <= 2; j++) {
            $("#star" + j.toString()).addClass("star-color");
        }
        for (var j = 3; j <= 5; j++) {
            $("#star" + j.toString()).removeClass("star-color");
        }
    });

    $("#star3").click(function () {
        $("#numb_star").val(3);
        for (var j = 1; j <= 3; j++) {
            $("#star" + j.toString()).addClass("star-color");
        }
        for (var j = 4; j <= 5; j++) {
            $("#star" + j.toString()).removeClass("star-color");
        }
    });

    $("#star4").click(function () {
        $("#numb_star").val(4);
        for (var j = 1; j <= 4; j++) {
            $("#star" + j.toString()).addClass("star-color");
        }
        $("#star5").removeClass("star-color");
    });

    $("#star5").click(function () {
        $("#numb_star").val(5);
        for (var j = 1; j <= 5; j++) {
            $("#star" + j.toString()).addClass("star-color");
        }
    });
})
$(document).ready(function () {
    $('.owl-carousel').owlCarousel({
        loop: true,
        margin: 0,
        nav: false,
        dots: false,
        autoplay: 2000,
        responsive: {
            0: {
                items: 1
            },
            600: {
                items: 1
            },
            1000: {
                items: 1
            }
        }
    })
    //Chuyen anh khi bam vao anh con ben duoi
    var img = new Array();
    for (var i = 0; i < 4; i++) {
        img[i] = document.getElementById("image" + (i + 1).toString()).getAttribute("src");
    }
    $('#image1').click(function () {
        $('#image1').addClass("active");
        $('#image2').removeClass("active");
        $('#image3').removeClass("active");
        $('#image4').removeClass("active");
        $('#anh-chinh').fadeOut(function () {
            $('#anh-chinh').attr("src", img[0]);
            $('#anh-chinh').fadeIn(1000);
        });
    })
    $('#image2').click(function () {
        $('#image2').addClass("active");
        $('#image1').removeClass("active");
        $('#image3').removeClass("active");
        $('#image4').removeClass("active");
        $('#anh-chinh').fadeOut(function () {
            $('#anh-chinh').attr('src', img[1]);
            $('#anh-chinh').fadeIn(1000);
        });
    })
    $('#image3').click(function () {
        $('#image3').addClass('active');
        $('#image2').removeClass('active');
        $('#image1').removeClass('active');
        $('#image4').removeClass('active');
        $('#anh-chinh').fadeOut(function () {
            $('#anh-chinh').attr('src', img[2]);
            $('#anh-chinh').fadeIn(1000);
        });
    })
    $('#image4').click(function () {
        $('#image4').addClass('active');
        $('#image2').removeClass('active');
        $('#image3').removeClass('active');
        $('#image1').removeClass('active');
        $('#anh-chinh').fadeOut(function () {
            $('#anh-chinh').attr('src', img[3]);
            $('#anh-chinh').fadeIn(1000);
        });
    })
    //nut tang so luong va giam so luong hang
});
$(document).ready(function () {
    $('#btn-cong').click(function () {
        var sl = $('#soluong').val();
        sl = parseInt(sl) + 1;
        $('#soluong').attr('value', sl.toString());
    });
    $('#btn-tru').click(function () {
        var sl = $('#soluong').val();
        if (sl > 1) {
            sl = parseInt(sl) - 1;
        } else {
            sl = 1;
        }
        $('#soluong').attr('value', sl.toString());
    });

})