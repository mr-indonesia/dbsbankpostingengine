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

//function view only
function ViewCompanyAgent(id) {

	$('#ModalCreateCompany').modal('show');

	// Fetch data from server
	$.get('/Company/Edit?id=' + id, function (data) {
		// Populate the form
		$('#Id').val(data.company.CompanyCode);
		$('#CompanyName').val(data.company.CompanyName);
		$('#Phone').val(data.company.Phone);
		$('#Fax').val(data.company.Fax);
		$('#Email').val(data.company.Email);
		$('#PIC1').val(data.company.Pic);
		$('#Address1').val(data.company.CompanyAddress);
		$('#PIC2').val(data.company.Pic2);
		$('#Address2').val(data.company.CompanyAddress2);
		$('#PicTitle').val(data.company.PicTitle);
		$('#kodya').val(data.company.KotaMadya);
		$('#RegisterDate').val(data.company.RegisterDate);
		$('#Npwp').val(data.company.Npwp);
		$('#ZipCode').val(data.company.ZipCode);
		//('#RoleAccessId').attr('readonly', 'readonly');
		$("#btnSaveCompany").hide();
		// Change modal title and form action
		$('#modalTitle').text('Edit Company');
		$('#companyAgentForm').attr('action', '/Company/UpdateCompanyAgent?id=' + id);

		// Populate dropdowns
		populateDropdown('#CompanyType', data.dropdowns.LstCompanyType, data.company.CompanyType);
		populateDropdown('#Category', data.dropdowns.LstCompanyCategory, data.company.CategoryCode);
		populateDropdown('#LobCode', data.dropdowns.LstCompanyLOB, data.company.LobCode);
		populateDropdown('#Propinsi', data.dropdowns.LstPropinsi, data.company.Propinsi);
		populateDropdown('#CoStatus', data.dropdowns.LstCompanyStatus, data.company.StatusCode);

		// Show the modal
		// $('#ModalCreateCompany').modal('show');
	});
}