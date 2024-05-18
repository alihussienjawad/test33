function DeleteItem(btn, tableId) {
	var table = document.getElementById(tableId);
	var rows = table.getElementsByTagName('tr');
	if (rows.length == 2) {
		alert("Cant delete last row");
		return;
	}
	$(btn).closest('tr').remove();
}



function AddItem(btn,tableId) {

	var table = document.getElementById(tableId);
	var rows = table.getElementsByTagName('tr');

	var rowOuterHtml = rows[rows.length - 1].outerHTML;

	var lastrowIdx = document.getElementById(`${tableId}hdnLastIndex`).value;

	var nextrowIdx = eval(lastrowIdx) + 1;

	document.getElementById(`${tableId}hdnLastIndex`).value = nextrowIdx;

	rowOuterHtml = rowOuterHtml.replaceAll('_' + lastrowIdx + '_', '_' + nextrowIdx + '_');
	rowOuterHtml = rowOuterHtml.replaceAll('[' + lastrowIdx + ']', '[' + nextrowIdx + ']');
	rowOuterHtml = rowOuterHtml.replaceAll('-' + lastrowIdx, '-' + nextrowIdx);


	var newRow = table.insertRow();
	newRow.innerHTML = rowOuterHtml;

	var x = table.getElementsByTagName("INPUT");
	for (var i = 0; i < x.length; i++) {
		if (x[i].type == "text" && x[i].id.indexOf('_' + nextrowIdx + '_') > 0)
			x[i].value = '';
	}
	var x1 = table.getElementsByTagName("TEXTAREA");
	for (var i = 0; i < x1.length; i++) {
		if (x1[i].type == "textarea" && x1[i].id.indexOf('_' + nextrowIdx + '_') > 0)
			x1[i].value = '';
	}

	//rebindValidator();

}
const rebindValidator = () => {
	var $form = $('#courseInsertForm');
	$form.unbind();
	$form.data("validator", null);
	$.validator.unobtrusive.parse($form);
	$form.validate($form.data("unobtrusiveValidation").options);
}
