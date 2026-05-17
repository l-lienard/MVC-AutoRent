<?php
class VehicleModel {
    private $file;

    public function __construct() {
        $this->file = 'disponible.csv';
    }

    public function importCsv($fileField) {
        if (isset($_FILES[$fileField]) && $_FILES[$fileField]['error'] == 0) {
            $tmp = $_FILES[$fileField]['tmp_name'];
            $dest = $this->file;

            $current = '';
            if (file_exists($dest)) {
                $current = file_get_contents($dest);
            }

            $storage = fopen($dest, "a");
            $import = fopen($tmp, "r");

            if ($import !== false && $storage !== false) {
                if (!empty($current) && substr($current, -1) !== "\n") {
                    fwrite($storage, "\n");
                }

                // Skip header of imported file
                fgetcsv($import, 1000, ";");

                while (($line = fgets($import)) !== false) {
                    if (trim($line) !== "") {
                        fwrite($storage, trim($line) . "\n");
                    }
                }

                fclose($import);
                fclose($storage);
                return true;
            }
        }
        return false;
    }

    public function getVehicles() {
        $vehicules = [];
        if (file_exists($this->file)) {
            $fichier = fopen($this->file, "r");
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
}

?>
