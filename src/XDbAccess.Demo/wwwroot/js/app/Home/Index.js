(function ($, window) {
    $(function () {
        $('#btnSingleTransDemo').click(function () {
            $.ajax({
                url: '/Home/SingleTransDemo',
                type: 'post',
                dataType: 'json',
                success: function (ret) {
                    alert(ret.message);
                    if (ret.status == 0) {
                        location.href = '/Home/QueryDemo';
                    }
                }
            });
        });

        $('#btnSingleTwoTransDemo').click(function () {
            $.ajax({
                url: '/Home/SingleTwoTransDemo',
                type: 'post',
                dataType: 'json',
                success: function (ret) {
                    alert(ret.message);
                    if (ret.status == 0) {
                        location.href = '/Home/QueryDemo';
                    }
                }
            });
        });

        $('#btnDoubleTwoTransDemo').click(function () {
            $.ajax({
                url: '/Home/DoubleTwoTransDemo',
                type: 'post',
                dataType: 'json',
                success: function (ret) {
                    alert(ret.message);
                    if (ret.status == 0) {
                        location.href = '/Home/QueryDemo';
                    }
                }
            });
        });

        $('#btnDoubleTwoTransNestRollbackDemo').click(function () {
            $.ajax({
                url: '/Home/DoubleTwoTransNestRollbackDemo',
                type: 'post',
                dataType: 'json',
                success: function (ret) {
                    alert(ret.message);
                    if (ret.status == 0) {
                        location.href = '/Home/QueryDemo';
                    }
                }
            });
        });

        $('#btnDoubleTwoTransRootRollbackDemo').click(function () {
            $.ajax({
                url: '/Home/DoubleTwoTransRootRollbackDemo',
                type: 'post',
                dataType: 'json',
                success: function (ret) {
                    alert(ret.message);
                    if (ret.status == 0) {
                        location.href = '/Home/QueryDemo';
                    }
                }
            });
        });
    });
})(jQuery, window)