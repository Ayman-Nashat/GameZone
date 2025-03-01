$(document).ready(function () {
    $('.js-delete').on('click', function () {
        var btn = $(this);

        $.ajax({
            url: `/Games/Delete/${btn.data('id')}`,
            method: 'Delete',
            success: function () {
                alert('success');
            },
            error: function () {
                alert('error');
            }
        });
    });
});