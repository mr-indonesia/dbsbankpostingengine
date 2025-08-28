$(function () {
	// Handle form submission
	$('#ModalRejectCompany form').on('submit', function (e) {
		e.preventDefault();

		var form = $(this);
		var url = form.attr('action');
		var formData = form.serialize();

		// Reset pesan error
		$('#rejectItemMsg').hide().empty();

		$.ajax({
			type: 'POST',
			url: url,
			data: formData,
			success: function (response) {
				if (response.success) {
					// Jika berhasil
					$('#rejectItemMsg')
						.removeClass('alert-danger')
						.addClass('alert-success')
						.html(response.message)
						.show();

					// Redirect atau refresh halaman setelah beberapa detik
					setTimeout(function () {
						$('#ModalRejectCompany').modal('hide');
						location.reload(); // atau window.location.href = redirectUrl;
					}, 2000);
				} else {
					// Jika gagal
					$('#rejectItemMsg')
						.removeClass('alert-success')
						.addClass('alert-danger')
						.html(response.message)
						.show();

					// Scroll ke pesan error
					$('.modal-body').animate({
						scrollTop: $('#rejectItemMsg').offset().top
					}, 500);
				}
			},
			error: function (xhr, status, error) {
				$('#rejectItemMsg')
					.removeClass('alert-success')
					.addClass('alert-danger')
					.html('Terjadi kesalahan pada server: ' + error)
					.show();
			}
		});
	});

	// Reset form dan pesan error ketika modal ditutup
	$('#ModalRejectCompany').on('hidden.bs.modal', function () {
		$(this).find('form')[0].reset();
		$('#rejectItemMsg').hide().empty();
	});
});