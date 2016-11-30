$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});
var validateFecha = function () {
    var init = $("input[name=finit1]").val();
    var end = $("input[name=fend1]").val();
    if (init == '' || end == '') {
        $('#alertMsg').text('Elegir rango de Fechas');
        $('#alertModal').modal('show');
        return false;
    }
    return true;
};
var busqueda = function () {
    if (!validateFecha()) return false;
    var filtro = {
        TipoDocumento: $('#mdb-select').val(),
        Serie: $('#serie').val(),
        Correlativo: $('#correlativo').val(),
        FechaInicial: $("input[name=finit1]").val(),
        FechaFinal: $("input[name=fend1]").val()
    };
    var url = "/Panel/Search";
    console.log(filtro);
    $.ajax(url, {
        data: filtro,
        method: "POST",
        error: function (xhr, ajaxOptions, thrownError) {
            $('#alertMsg').text(thrownError);
            $('#alertModal').modal('show');
        }
    }).done(function (data) {
        $('#tableContent').html(data);
    });
    return false;
};

$(document).ready(function () {
    $('#myTable').DataTable({
        responsive: true,
        bProcessing: false,
        bFilter: false,
        bAutoWidth: false,
        bLengthChange: false
    });
    $('#finit').pickadate({
        formatSubmit: 'dd/mm/yyyy',
        hiddenSuffix: '1'
    });
    $('#fend').pickadate({
        formatSubmit: 'dd/mm/yyyy',
        hiddenSuffix: '1'
    });
    $('#mdb-select').material_select();
    $('#formSearch').submit(busqueda);
});