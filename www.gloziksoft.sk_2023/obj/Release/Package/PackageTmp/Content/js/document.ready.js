$(document).ready(function () {
    // Submenu - handling hover for dropdown
    $('ul.nav li.dropdown').hover(
        function () { $(this).addClass('open'); },
        function () { $(this).removeClass('open'); }
    );


    // Toggle navigation icon
    $('#navbar').on('show.bs.collapse', function () {
        $('#menuIcon').removeClass('fa-bars').addClass('fa-times');
    });

    $('#navbar').on('hide.bs.collapse', function () {
        $('#menuIcon').removeClass('fa-times').addClass('fa-bars');
    });

    $('#navbar-product').on('show.bs.collapse', function () {
        $('#menuIconProtect').removeClass('fa-bars').addClass('fa-times');
    });

    $('#navbar-product').on('hide.bs.collapse', function () {
        $('#menuIconProtect').removeClass('fa-times').addClass('fa-bars');
    });
})
