$(function () {
	// Handle form submission
	$('#ModalApproveCompany form').on('submit', function (e) {
		e.preventDefault();

		var form = $(this);
		var url = form.attr('action');
		var formData = form.serialize();

		// Reset pesan error
		$('#approveItemMsg').hide().empty();

		$.ajax({
			type: 'POST',
			url: url,
			data: formData,
			success: function (response) {
				if (response.success) {
					// Jika berhasil
					$('#approveItemMsg')
						.removeClass('alert-danger')
						.addClass('alert-success')
						.html(response.message)
						.show();

					// Redirect atau refresh halaman setelah beberapa detik
					setTimeout(function () {
						$('#ModalApproveCompany').modal('hide');
						location.reload(); // atau window.location.href = redirectUrl;
					}, 2000);
				} else {
					// Jika gagal
					$('#approveItemMsg')
						.removeClass('alert-success')
						.addClass('alert-danger')
						.html(response.message)
						.show();

					// Scroll ke pesan error
					$('.modal-body').animate({
						scrollTop: $('#approveItemMsg').offset().top
					}, 500);
				}
			},
			error: function (xhr, status, error) {
				$('#approveItemMsg')
					.removeClass('alert-success')
					.addClass('alert-danger')
					.html('Terjadi kesalahan pada server: ' + error)
					.show();
			}
		});
	});

	// Reset form dan pesan error ketika modal ditutup
	$('#ModalApproveCompany').on('hidden.bs.modal', function () {
		$(this).find('form')[0].reset();
		$('#approveItemMsg').hide().empty();
	});
});

function ConfirmApproveCompanyAgent(id) {

	$('#ModalApproveCompany').modal('show');
	$('#coCode').val(id);
	$('#ApproveTitle').text('Approve Company');
	$('#approveCompanyForm').attr('action', '/Company/ApproveCompanyAgent?id=' + id);
}

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

function ConfirmRejectCompanyAgent(id) {
	$('#ModalRejectCompany').modal('show');
	$('#rejectCode').val(id);
	$('#RejectTitle').text('Reject Company');
	$('#rejectCompanyForm').attr('action', '/Company/RejectCompanyAgent?id=' + id);
}