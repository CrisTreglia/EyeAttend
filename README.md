# Face Recognition with API Integration (C# & Python)

This project combines a C# API for student photo management with a Python-based real-time facial recognition system. The Python script fetches student photos from the C# API, which sources its data from a database, to enhance the recognition model dynamically.

## Key Features

* **C# API with Database Integration:** Manages and serves student photos from a database in MSSQL.
* **Real-time Face Detection:**  Identifies faces from a webcam feed using OpenCV.
* **API-Driven Model Updates:** Fetches student photos from the C# API and dynamically incorporates them into the recognition model.
* **Error Handling:** Includes robust error handling for API requests, database interactions, and file operations.

## Requirements

* **C# Side:**
    * .NET SDK 
    * [Your Preferred C# Framework] (e.g., ASP.NET Core)
    * [Your Chosen Database] (e.g., SQL Server, MySQL, PostgreSQL)
* **Python Side:**
    * Python (3.x recommended)
    * OpenCV (`opencv-contrib-python`)
    * `requests` library

## Installation & Setup

1. **C# API:**
   * Set up the database schema to store student information and photo paths/data.
   * Build and deploy the C# API according to its instructions. Ensure it has an endpoint to retrieve student photos (e.g., `/api/StudentPhotoesApi/student/{student_id}/files`).
2. **Python Script:**
   * Clone this repository: `git clone https://your-github-repo-url.git`
   * Install Python dependencies: `pip install opencv-contrib-python requests`
   * Train a facial recognition model (if you haven't already) and save it as `trainer/trainer.yml`.

## Usage

1. **Configure API Endpoint:**
   * Update the `api_base_url` variable in `main.py` to match the URL of your deployed C# API.
   * Make sure the API endpoint and image paths are correct.
2. **Populate Database:**
   * Add student information and photo data to your database.
3. **Run the Python Script:** `python main.py`

## Configuration

* Adjust `student_ids` in `main.py` to match the IDs of the students you want to recognize.
* Customize the C# API and database schema to fit your specific data storage and retrieval requirements.

## Contributing

Contributions are welcome! Feel free to open issues or submit pull requests.

## License

This project is licensed under the [MIT License](LICENSE). 
