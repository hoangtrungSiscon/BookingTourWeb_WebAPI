pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'bookingtourwebapi' // Tên Docker image
        DOCKER_TAG = 'latest'            // Tag của Docker image
    }

    stages {
        stage('Checkout') {
            steps {
                git branch: 'master', url: 'https://github.com/hoangtrungSiscon/Tour-Booking-Web-Backend.git'
            }
        }

        stage('Restore Packages') {
            steps {
                bat '"C:\\Program Files\\dotnet\\dotnet.exe" restore BookingTourWeb_WebAPI.sln'
            }
        }

        stage('Build') {
            steps {
                bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" BookingTourWeb_WebAPI.sln /p:Configuration=Release'
            }
        }

        stage('Docker Build') {
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

        stage('Docker Run') {
            steps {
                script {
                    def dockerRunCmd = "docker run -d -p 8080:80 --name bookingtourwebapi ${DOCKER_IMAGE}:${DOCKER_TAG}"
                    if (isUnix()) {
                        sh dockerRunCmd
                    } else {
                        bat dockerRunCmd
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
