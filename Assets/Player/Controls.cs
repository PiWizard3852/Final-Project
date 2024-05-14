using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Vehicles;

namespace Player
{
    public class Controls : MonoBehaviour
    {
        public TextMeshProUGUI currentScoreText;
        
        private GameState _gameState;
            
        private bool _lastPress;
        private KeyCode _lastKey;

        private int _lastLog;
        private float _logOffset;

        private bool _lost;
        
        public void Start()
        {
            _gameState = GameObject.FindGameObjectWithTag("GameState").gameObject.GetComponent<GameState>();
            
            _gameState.currentScoreText = currentScoreText;
            _gameState.currentScore = 0;
            
            _gameState.isOriginal = true;

            var playerTransform = transform;

            playerTransform.position = new Vector3(0, 1.5f, 0);
            playerTransform.rotation = Quaternion.Euler(0, 180, 0);

            _lastPress = false;
        }

        public void Update()
        {
            var playerTransform = transform;
            var position = playerTransform.position;

            if (transform.position.z > _gameState.currentScore)
            {
                _gameState.currentScore = (int) transform.position.z;
            }
            
            Quaternion rotation;

            if (!Input.GetKey(_lastKey)) _lastPress = false;

            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (!_lastPress)
                {
                    rotation = Quaternion.Euler(0, 180, 0);
                    playerTransform.rotation = rotation;

                    position = new Vector3(position.x, position.y, position.z + 1);
                    playerTransform.position = position;
                }

                _lastPress = true;
                _lastKey = KeyCode.UpArrow;
            }

            if (Input.GetKey(KeyCode.DownArrow) && _gameState.currentScore - transform.position.z < 3)
            {
                if (!_lastPress)
                {
                    rotation = Quaternion.Euler(0, 0, 0);
                    playerTransform.rotation = rotation;

                    position = new Vector3(position.x, position.y, position.z - 1);
                    playerTransform.position = position;
                }

                _lastPress = true;
                _lastKey = KeyCode.DownArrow;
            }

            if (Input.GetKey(KeyCode.LeftArrow))
            {
                if (!_lastPress)
                {
                    rotation = Quaternion.Euler(0, 90, 0);
                    playerTransform.rotation = rotation;

                    _logOffset--;

                    position = new Vector3(position.x - 1, position.y, position.z);
                    playerTransform.position = position;
                }

                _lastPress = true;
                _lastKey = KeyCode.LeftArrow;
            }

            if (Input.GetKey(KeyCode.RightArrow))
            {
                if (!_lastPress)
                {
                    rotation = Quaternion.Euler(0, 270, 0);
                    playerTransform.rotation = rotation;

                    _logOffset++;
                    
                    position = new Vector3(position.x + 1, position.y, position.z);
                    playerTransform.position = position;
                }

                _lastPress = true;
                _lastKey = KeyCode.RightArrow;
            }
            
            var grasses = GameObject.FindGameObjectsWithTag("Grass");

            foreach (var grass in grasses)
            {
                if (transform.position.z - grass.transform.position.z > 15)
                {
                    Destroy(grass);
                }
            }
            
            var roads = GameObject.FindGameObjectsWithTag("Road");

            foreach (var road in roads)
            {
                if (transform.position.z - road.transform.position.z > 15)
                {
                    Destroy(road);
                }
            }
            
            var rivers = GameObject.FindGameObjectsWithTag("River");

            foreach (var river in rivers)
            {
                if (transform.position.z - river.transform.position.z > 15)
                {
                    Destroy(river);
                }
            }
            
            var railroads = GameObject.FindGameObjectsWithTag("Railroad");

            foreach (var railroad in railroads)
            {
                if (transform.position.z - railroad.transform.position.z > 15)
                {
                    Destroy(railroad);
                }
            }
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Car") || collision.gameObject.CompareTag("River") || collision.gameObject.CompareTag("Train"))
            {
                Lose();
            }
        }

        public void OnBecameInvisible()
        {
            Lose();
        }

        public void OnCollisionStay(Collision collision)
        {
            if (collision.gameObject.CompareTag("Log"))
            {
                if (!_lastLog.Equals(collision.gameObject.GetHashCode()))
                {
                    _lastLog = collision.gameObject.GetHashCode();
                    _logOffset = transform.position.x - collision.transform.position.x;
                }
                else
                {
                    var playerTransform = transform;
                    var playerPosition = playerTransform.position;
                    playerPosition = new Vector3(collision.transform.position.x + _logOffset, playerPosition.y,
                        playerPosition.z);
                    playerTransform.position = playerPosition;
                }
            }
            else
            {
                _lastLog = -1;
            }
        }

        private void Lose()
        {
            if (!_lost)
            {
                _gameState.totalScore += _gameState.currentScore;
                
                SceneManager.LoadScene(0);
            }

            _lost = true;
        }
    }
}