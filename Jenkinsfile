pipeline {
    agent any

    environment {
        DOCKER_IMAGE = 'bookingtourwebapi' // Docker image name
        DOCKER_TAG = 'latest'             // Docker image tag
    }

    stages {
        // Checkout stage
        stage('Checkout') {
            steps {
                git branch: 'master', 
                    url: 'https://github.com/hoangtrungSiscon/Tour-Booking-Web-Backend.git', 
                    credentialsId: '3338ef97-97e2-48cf-92d4-b2318012413b'
            }
        }

        // Restore packages
        stage('Restore Packages') {
            steps {
                bat '"C:\\Program Files\\dotnet\\dotnet.exe" restore BookingTourWeb_WebAPI.sln'
            }
        }

        // Build application
        stage('Build') {
            steps {
                bat '"C:\\Program Files\\Microsoft Visual Studio\\2022\\Community\\MSBuild\\Current\\Bin\\MSBuild.exe" BookingTourWeb_WebAPI.sln /t:Restore,Build /p:Configuration=Release /p:WarningsAsErrors=false'
            }
        }

        // Test stage
        stage('Run Tests') {
            steps {
                bat '"C:\\Program Files\\dotnet\\dotnet.exe" test BookingTourWeb_WebAPI.sln --logger trx'
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

        // Run Docker container
        stage('Run Docker Container') {
            steps {
                script {
                    // Check if the container exists (running or stopped)
                    def checkContainerCmd = "docker ps -a -q -f name=bookingtourwebapi"
                    def containerExists = isUnix() ? sh(script: checkContainerCmd, returnStdout: true).trim() : bat(script: checkContainerCmd, returnStdout: true).trim()

                    if (containerExists) {
                        echo "Container 'bookingtourwebapi' exists. It may be stopped, skipping creation."
                    } else {
                        echo "Container 'bookingtourwebapi' does not exist. Creating a new one."

                        // Run the Docker container if it doesn't exist
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

        // Refresh Docker container
        // stage('Refresh Docker Container') {
        //     steps {
        //         script {
        //             def checkContainerCmd = "docker ps -q -f name=bookingtourwebapi"
        //             def containerExists = isUnix() ? sh(script: checkContainerCmd, returnStdout: true).trim() : bat(script: checkContainerCmd, returnStdout: true).trim()
        
        //             if (containerExists) {
        //                 echo "Container 'bookingtourwebapi' is already running. Restarting it."
        
        //                 // Command to restart the container with the updated image
        //                 def restartCmd = "docker container restart bookingtourwebapi"
        //                 if (isUnix()) {
        //                     sh restartCmd
        //                 } else {
        //                     bat restartCmd
        //                 }
        //             } else {
        //                 echo "Container 'bookingtourwebapi' is not running. Starting a new container."
        
        //                 def dockerRunCmd = "docker run -d -p 8081:80 --name bookingtourwebapi ${DOCKER_IMAGE}:${DOCKER_TAG}"
        //                 if (isUnix()) {
        //                     sh dockerRunCmd
        //                 } else {
        //                     bat dockerRunCmd
        //                 }
        //             }
        //         }
        //     }
        // }

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
