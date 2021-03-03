﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WeekAnkama
{
    public class PathRequestManager : MonoBehaviour
    {
        Queue<PathRequest> pathRequestQueue = new Queue<PathRequest>();
        PathRequest currentPathRequest;

        static PathRequestManager instance;
        Pathfinding pathfinding;

        bool isProcessingPath;

        void Awake()
        {
            instance = this;
            pathfinding = GetComponent<Pathfinding>();
        }

        public static void RequestPath(Vector3 pathStart, Vector3 pathEnd, int maxDistance, Action<Vector3[], bool> callback)
        {
            PathRequest newRequest = new PathRequest(pathStart, pathEnd, maxDistance, callback);
            instance.pathRequestQueue.Enqueue(newRequest);
            instance.TryProcessNext();
        }

        void TryProcessNext()
        {
            if (!isProcessingPath && pathRequestQueue.Count > 0)
            {
                currentPathRequest = pathRequestQueue.Dequeue();
                isProcessingPath = true;
                pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd, currentPathRequest.maxDistance);
            }
        }

        public void FinishedProcessingPath(Vector3[] path, bool success)
        {
            currentPathRequest.callback(path, success);
            isProcessingPath = false;
            TryProcessNext();
        }


        struct PathRequest
        {
            public Vector3 pathStart;
            public Vector3 pathEnd;
            public int maxDistance;
            public Action<Vector3[], bool> callback;

            public PathRequest(Vector3 _start, Vector3 _end, int _maxDistance, Action<Vector3[], bool> _callback)
            {
                pathStart = _start;
                pathEnd = _end;
                callback = _callback;
                maxDistance = _maxDistance;
            }

        }
    }
}
