pipeline {
    agent any

    stages {
        stage('Checkout') {
            steps {
                // Lấy mã nguồn từ GitHub
                git branch: 'master', url: 'https://github.com/hoangtrungSiscon/Tour-Booking-Web-Backend.git'
            }
        }
        stage('Restore Packages') {
            steps {
                // Khôi phục các package NuGet
                bat '"C:\\Program Files\\dotnet\\dotnet.exe" restore BookingTourWeb_WebAPI.sln'
            }
        }
        stage('Build') {
            steps {
                // Build project (Windows)
                bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" BookingTourWeb_WebAPI.sln /p:Configuration=Release'
                
                // Hoặc Linux (DotNet CLI)
                // sh 'dotnet build BookingTourWeb_WebAPI.sln --configuration Release'
            }
        }

        stage('Test') {
            steps {
                // Thực hiện kiểm tra nếu có
                echo 'Running tests...'
                // Ví dụ: sh 'dotnet test'
            }
        }

        stage('Deploy') {
            steps {
                // Triển khai ứng dụng (ví dụ: copy file đến server, chạy script)
                echo 'Deploying application...'
                // Ví dụ: sh './deploy.sh'
            }
        }
    }

    post {
        success {
            echo 'Build and Deploy Successful!'
        }
        failure {
            echo 'Build or Deploy Failed!'
        }
    }
}
