$(function () {
    //Datemask dd/mm/yyyy
    $('#datemask').inputmask('dd/mm/yyyy', { 'placeholder': 'dd/mm/yyyy' })
    //Datemask2 mm/dd/yyyy
    $('#datemask2').inputmask('mm/dd/yyyy', { 'placeholder': 'mm/dd/yyyy' })
    
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
    
    //Date and time picker
    $('#reservationdatetime').datetimepicker({ icons: { time: 'far fa-clock' } });

    //Timepicker
    $('#timepicker').datetimepicker({
        format: 'LT'
    })
})
