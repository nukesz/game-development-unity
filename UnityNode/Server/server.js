var io = require('socket.io')(process.env.PORT || 3000);
var shortid = require('shortid');

console.log('server started');

var players = [];
var playerSpeed = 3;

io.on('connection', function(socket) {
  var thisPlayerId = shortid.generate();

  var player = {
    id: thisPlayerId,
    destination: {
      x: 0,
      y: 0     
    },
    lastPosition: {
      x: 0,
      y: 0     
    },
    lastMoveTime: 0
  };

  players[thisPlayerId] = player;

  console.log('client connected, broadcasting spawn id: ' + thisPlayerId);

  socket.emit('register', { id: thisPlayerId });
  socket.broadcast.emit('spawn', { id: thisPlayerId });
  socket.broadcast.emit('requestPosition');


  for (var playerId in players) {
    if (playerId === thisPlayerId) {
      continue;
    }
    socket.emit('spawn', players[playerId]);
    console.log('sending spawn with the new player id: ' + playerId);
  }


  socket.on('move', function(data) {
    data.id = thisPlayerId;
    console.log('client moved: ', JSON.stringify(data));
    player.destination = data.d;

    console.log("distance between current and destination:", 
      lineDistance(player.lastPosition, player.destination));
    var elapsedTime = Date.now() - player.lastMoveTime;
    var travelDistanceLimit = elapsedTime * playerSpeed / 1000;
    var requestedDistanceTraveled = lineDistance(player.lastPosition, data.c);


    console.log("travelDistanceLimit:", travelDistanceLimit);
    console.log("requestedDistanceTraveled:", requestedDistanceTraveled);
    if (requestedDistanceTraveled > travelDistanceLimit) {
      // We know they are cheating
    }

    player.lastMoveTime = Date.now();

    player.lastPosition = data.c;
    delete data.c;
    data.x = data.d.x;
    data.y = data.d.y;
    delete data.d;

    socket.broadcast.emit('move', data);
  });

  socket.on('follow', function(data) {
    console.log('follow request: ', data);
    data.id = thisPlayerId;
    socket.broadcast.emit('follow', data);
  });

  socket.on('updatePosition', function(data) {
    console.log('update position: ', data);
    data.id = thisPlayerId;
    socket.broadcast.emit('updatePosition', data);
  });

  socket.on('attack', function(data) {
    console.log('attack request: ', data);
    data.id = thisPlayerId;
    io.emit('attack', data);
  });

  socket.on('disconnect', function() {
    console.log('client disconnected');
    delete players[thisPlayerId];
    socket.broadcast.emit('disconnected', { id: thisPlayerId });
  });


});

function lineDistance(vectorA, vectorB) {
  var xs = vectorB.x - vectorA.x;
  var ys = vectorB.y - vectorA.y;
  xs = xs * xs;
  ys = ys * ys;
  return Math.sqrt(xs + ys); 
}