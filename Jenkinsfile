pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'bookingtourwebapi' // Tên Docker image
        DOCKER_TAG = 'latest'            // Tag của Docker image
    }

    stages {
        // Lấy mã nguồn từ GitHub
        stage('Checkout') {
            steps {
                git branch: 'master', 
                    url: 'https://github.com/hoangtrungSiscon/Tour-Booking-Web-Backend.git', 
                    credentialsId: '3338ef97-97e2-48cf-92d4-b2318012413b'
            }
        }

        // Khôi phục các package .NET
        stage('Restore Packages') {
            steps {
                bat '"C:\\Program Files\\dotnet\\dotnet.exe" restore BookingTourWeb_WebAPI.sln'
            }
        }

        // Build ứng dụng với MSBuild, ignoring warnings
        stage('Build') {
            steps {
                bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" BookingTourWeb_WebAPI.sln /t:Restore,Build /p:Configuration=Release /p:WarningsAsErrors=false'

            }
        }

        

        // Build Docker image
        stage('Build Docker Image') {
    steps {
        script {
            def dockerCmd = "docker build -t ${DOCKER_IMAGE}:${DOCKER_TAG} ."
            if (isUnix()) {
                sh dockerCmd
            } else {
                bat dockerCmd
            }
        }
    }
}

stage('Run Docker Container') {
    steps {
        script {
            // Check if container is already running
            def checkContainerCmd = "docker ps -q -f name=bookingtourwebapi"
            def containerExists = isUnix() ? sh(script: checkContainerCmd, returnStdout: true).trim() : bat(script: checkContainerCmd, returnStdout: true).trim()
            
            if (containerExists) {
                echo "Container 'bookingtourwebapi' is already running. Skipping creation."
            } else {
                def dockerRunCmd = "docker run -d -p 8081:80 --name bookingtourwebapi ${DOCKER_IMAGE}:${DOCKER_TAG}"
                if (isUnix()) {
                    sh dockerRunCmd
                } else {
                    bat dockerRunCmd
                }
            }
        }
    }
}

    }

    post {
        success {
            echo 'Docker Deployment Successful!'
        }
        failure {
            echo 'Docker Deployment Failed!'
        }
    }
}
