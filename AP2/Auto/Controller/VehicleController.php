<?php
require_once 'Model/VehicleModel.php';

$model = new VehicleModel();

if (isset($_POST['submit_import']) && isset($_FILES['file_csv'])) {
    if ($model->importCsv('file_csv')) {
        header("Location: index.php?success=1");
        exit();
    }
}

$vehicules = $model->getVehicles();

?>
