$(function () {
    //Datemask dd/mm/yyyy
    $('#RentalDate').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
    $('#RentReturnDate').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
    $('#RentRealReturnDate').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })

    //Date picker
    $('#RentalDate').datetimepicker({
        format: 'L'
    });
    $('#RentReturnDate').datetimepicker({
        format: 'L'
    });
    $('#RentRealReturnDate').datetimepicker({
        format: 'L'
    });
    
})
