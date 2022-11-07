$(function () {

    //Date picker
    $('#RentalDate').datetimepicker({
        format: 'DD-MM-YYYY',
        useCurrent: true
    });

    $('#RentReturnDate').datetimepicker({
        format: 'DD-MM-YYYY',
        useCurrent: false
    });

    $('#RentRealReturnDate').datetimepicker({
        format: 'DD-MM-YYYY',
        useCurrent: false
    });
    
})

