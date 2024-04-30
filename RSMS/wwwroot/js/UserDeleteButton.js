$(document).ready(function () {
    $('.delete-btn').click(function () {
        var userId = $(this).data('userId');

        // Confirm deletion
        if (confirm("Are you sure you want to delete this user?")) {
            // Send AJAX request to delete user
            $.ajax({
                url: '/Admin/DeleteUser',
                type: 'POST',
                data: { id: userId },
                success: function (response) {
                    if (response.success) {
                        // Remove the deleted record from the table
                        $('#recordTable').find('[data-id="' + userId + '"]').closest('tr').remove();
                        alert('User deleted successfully.');
                    } else {
                        alert('Failed to delete user.');
                    }
                },
                error: function () {
                    alert('An error occurred while processing your request.');
                }
            });
        }
    });
});