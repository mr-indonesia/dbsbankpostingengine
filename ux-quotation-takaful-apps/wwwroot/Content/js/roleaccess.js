function editRoleAccess(id) {

	// Fetch data from server
	$.get('/RoleAccess/Edit?id=' + id, function (data) {
		// Populate the form
		$('#Id').val(data.RoleAccessID);
		$('#RoleAccessId').val(data.RoleAccessID);
		$('#RoleAccessName').val(data.RoleAccessName);
		$('#RoleAccessId').attr('readonly', 'readonly');

		// Change modal title and form action
		$('#modalTitle').text('Edit Role Access');
		$('#roleAccessForm').attr('action', '/RoleAccess/UpdateRoleAccess?id=' + id);

		// Show the modal
		$('#ModalRoleAccess').modal('show');
	});
}

// Function to reset modal for adding new record
function addRoleAccess() {
	// Reset form and set default values
	$('#Id').val(0);
	$('#RoleAccessId').val('');
	$('#RoleAccessName').val('');
	$('#RoleAccessId').removeAttr('readonly');

	// Change modal title and form action
	$('#modalTitle').text('Add Role Access');
	$('#roleAccessForm').attr('action', '/RoleAccess/CreateRoleAccess');

	// Show the modal
	$('#ModalRoleAccess').modal('show');
}