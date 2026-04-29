<?php
class VehicleModel {
    private $csvFile = 'disponible.csv';

    public function getAllVehicles() {
        $vehicules = [];
        if (file_exists($this->csvFile)) {
            $fichier = fopen($this->csvFile, "r");
            if ($fichier !== false) {
                $entetes = fgetcsv($fichier, 1000, ";");
                while (($ligne = fgetcsv($fichier, 1000, ";")) !== false) {
                    if (count($entetes) == count($ligne)) {
                        $vehicules[] = array_combine($entetes, $ligne);
                    }
                }
                fclose($fichier);
            }
        }
        return $vehicules;
    }

    public function importVehicles($filePath) {
        $contenuActuel = file_get_contents($this->csvFile);
        $stockage = fopen($this->csvFile, "a");
        $import = fopen($filePath, "r");

        if ($import !== false && $stockage !== false) {
            if (!empty($contenuActuel) && substr($contenuActuel, -1) !== "\n") {
                fwrite($stockage, "\n");
            }
            fgetcsv($import, 1000, ";");
            while (($ligne = fgets($import)) !== false) {
                if (trim($ligne) !== "") {
                    fwrite($stockage, trim($ligne) . "\n");
                }
            }
            fclose($import);
            fclose($stockage);
            return true;
        }
        return false;
    }
}
?>