(function ($, window) {
    $(function () {
        $('.removeUser').click(function () {
            if (confirm('Are you sure?')) {
                var id = $(this).attr('userId');
                var url = '/Home/RemoveUser?id=' + id;
                $.ajax({
                    url: url,
                    type: 'post',
                    dataType: 'json',
                    success: function (ret) {
                        if (ret.status == 0) {
                            location.href = location.href;
                        }
                        alert(ret.message);
                    }
                });
            }
        });
    });
})(jQuery, window);