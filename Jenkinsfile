pipeline {
    agent any
    
    stages {
        
        stage('Deploy backend') {
            steps {
                dir('/home/malde/projects/BackEnd') {
                    sh 'git fetch origin main && git reset --hard origin/main'
                    sh 'docker build -t backend-image .'
                }
                
                dir('/home/malde/projects/infrastructure') {
                    sh 'docker compose up -d --build'
                    sh 'docker image prune -f'
                }
            }
        }

      stage('Database Update') {
            steps {
                echo "Venter på at databasen starter op..."
                sleep time: 10, unit: 'SECONDS'
                
                sh 'docker cp /home/malde/projects/BackEnd/src/database_update.sql db:/tmp/db.sql'
                
                sh 'docker exec db psql -U admin -d db -f /tmp/db.sql'
                
                sh 'docker exec db rm /tmp/db.sql'
            }
        }
    }
}