<?php
require_once 'Controller/VehicleController.php';

// Récupération du type sélectionné pour le filtrage
$typeSelectionne = isset($_GET['type_voiture']) ? $_GET['type_voiture'] : "";

include 'View/navbar.php';

include 'View/index.php';
?>