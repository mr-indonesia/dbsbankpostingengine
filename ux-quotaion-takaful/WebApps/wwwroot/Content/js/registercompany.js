$(function () {
	// Handle form submission
	$('#ModalCreateCompany form').on('submit', function (e) {
		e.preventDefault();

		var form = $(this);
		var url = form.attr('action');
		var formData = form.serialize();

		// Reset pesan error
		$('#addItemMsg').hide().empty();

		$.ajax({
			type: 'POST',
			url: url,
			data: formData,
			success: function (response) {
				if (response.success) {
					// Jika berhasil
					$('#addItemMsg')
						.removeClass('alert-danger')
						.addClass('alert-success')
						.html(response.message)
						.show();

					// Redirect atau refresh halaman setelah beberapa detik
					setTimeout(function () {
						$('#ModalCreateCompany').modal('hide');
						location.reload(); // atau window.location.href = redirectUrl;
					}, 2000);
				} else {
					// Jika gagal
					$('#addItemMsg')
						.removeClass('alert-success')
						.addClass('alert-danger')
						.html(response.message)
						.show();

					// Scroll ke pesan error
					$('.modal-body').animate({
						scrollTop: $('#addItemMsg').offset().top
					}, 500);
				}
			},
			error: function (xhr, status, error) {
				$('#addItemMsg')
					.removeClass('alert-success')
					.addClass('alert-danger')
					.html('Terjadi kesalahan pada server: ' + error)
					.show();
			}
		});
	});

	// Reset form dan pesan error ketika modal ditutup
	$('#ModalCreateCompany').on('hidden.bs.modal', function () {
		$(this).find('form')[0].reset();
		$('#addItemMsg').hide().empty();
	});
});


function addCompanyAgent() {
	// Reset form and set default values
	$('#Id').val('');

	// Change modal title and form action
	$('#modalTitle').text('Register Company');
	$('#companyAgentForm').attr('action', '/Company/CreateCompanyAgent');

	// Show the modal
	$('#ModalCreateCompany').modal('show');
}

function editCompanyAgent(id) {

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

		$("#btnSaveCompany").show();

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

// Function to populate dropdown
function populateDropdown(selector, options, selectedValue) {
	var dropdown = $(selector);
	dropdown.empty();
	//dropdown.append($('<option>').val('').text('Select...'));

	$.each(options, function (index, option) {
		dropdown.append($('<option>').val(option.Value).text(option.Text));
	});

	if (selectedValue) {
		dropdown.val(selectedValue);
	}
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