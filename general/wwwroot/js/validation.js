$().ready(function () {

    function checkalphabet(a) {
        $(a).keyup(function (e) {
            if (!($(this).val().substring($(this).val().length - 1, $(this).val().length) >= "a" && $(this).val().substring($(this).val().length - 1, $(this).val().length) <= "z") && !($(this).val().substring($(this).val().length - 1, $(this).val().length) >= "A" && $(this).val().substring($(this).val().length - 1, $(this).val().length) <= "Z") && $(this).val().substring($(this).val().length - 1, $(this).val().length) != " ")
                $(this).val($(this).val().substring(0, $(this).val().length - 1));
        });
    }

    function checknumber(a) {
        $(a).keyup(function (e) {
            if (!($(this).val().substring($(this).val().length - 1, $(this).val().length) >= "0" && $(this).val().substring($(this).val().length - 1, $(this).val().length) <= "9"))
                $(this).val($(this).val().substring(0, $(this).val().length - 1));
        });
    }

    function checklength(a) {
        $(a).keyup(function (e) {
            if ($(this).val().length >= 18) {
                $(this).val($(this).val().substring(0, $(this).val().length - 1));
            }
        });
    }

    function checkrequired(a) {
        $(a).blur(function (e) {
            if ($(this).val().length == 0) {
                $(this).focus();
            }
        });
    }

    function checkrange(a) {
        $(a).keyup(function (e) {
            if ($(this).val() < 0 || $(this).val() > 100) {
                $(this).val($(this).val().substring(0, $(this).val().length - 1));
            }
        });
    }

    function checkdate(a) {
        $(a).keyup(function (e) {
            months = [31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31];
            if (($(this).val().substring(0, 4) % 4 == 0 && $(this).val().substring(0, 4) % 100 != 0) || $(this).val().substring(0, 4) % 400 == 0) {
                months[1] = 29;
            }
            if ($(this).val().substring(8, 10) > months[$(this).val().substring(5, 7) - 1])
                $(this).val($(this).val().substring(0, $(this).val().length - 1));
        });
    }

    checkalphabet("#name");

    checkdate("#dateofbirth");

    checknumber("#marks");

    checklength("#name");

    checkrequired("#name");

    checkrange("#marks");

    checkdate("#startdate");

    checkdate("#enddate");

    checknumber("#commission");

    checknumber("#experience");
});