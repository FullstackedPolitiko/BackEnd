pipeline {
    agent any
    stages {
        stage('Deploy backend') {
           
            steps {
                dir('/home/malde/projects/BackEnd') {
                   sh 'git fetch origin main'
                   sh 'git reset --hard origin/main'
                   sh 'docker build -t backend-image .'
                }
                
                dir('/home/malde/projects/infrastructure') {
                   sh 'docker compose --version'    
                   sh 'docker compose up -d --build'
                   sh 'docker image prune -f'
                }
            }
        }
    }
}