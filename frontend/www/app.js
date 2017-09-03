// *************************************************************
// App world
// *************************************************************

var Hand = {
   object        : null,             // Image or any other object
   positions     : [ { x: 0, y: 0},  // Start
                     { x: 0, y: 0},  // Pos 1
                     { x: 0, y: 0}   // Pos 2
                   ],
   actpos        : { x: 0, y: 0},    // Actual position
   prev_position : 0,                // Array id of prev position
   next_position : 0,                // Array id of next position
   timer         : null              // timer object for kill it
}

var Sun = {
   object        : null,             // Image or any other object
   positions     : [ { x: 0, y: 0},  // Start
                     { x: 0, y: 0},  // Pos 1
                     { x: 0, y: 0},  // Pos 2
                     { x: 0, y: 0}   // Pos 3
                   ],
   actpos        : { x: 0, y: 0},    // Actual position
   prev_position : 0,                // Array id of prev position
   next_position : 0,                // Array id of next position
   timer         : null              // timer object for kill it
}

var W1 = {
   object        : null,             // Image or any other object
   timer         : null              // timer object for kill it
}

var W2 = {
   object        : null,             // Image or any other object
   timer         : null              // timer object for kill it
}

var W3 = {
   object        : null,             // Image or any other object
   timer         : null              // timer object for kill it
}

var W4 = {
   object        : null,             // Image or any other object
   timer         : null              // timer object for kill it
}

var START =  0;
var STOP  = -1;

var animations = {
   sun : [ [ { todo : 'show', obj : W1,             time : 5000 } ],
           [ { todo : 'move', obj : Hand, pos : 1,  time : 2000 } ],
           [ { todo : 'move', obj : Sun,  pos : 1,  time : 1000 } ],
           [ { todo : 'show', obj : W2,             time : 5000 } ],
           [ { todo : 'show', obj : W3,             time : 5000 } ],
           [ { todo : 'move', obj : Hand, pos : 2,  time : 1000 },
             { todo : 'move', obj : Sun,  pos : 2,  time : 1000 } ],
           [ { todo : 'move', obj : Hand, pos : 1,  time : 1000 },
             { todo : 'move', obj : Sun,  pos : 3,  time : 1000 } ],
           [ { todo : 'move', obj : Hand, pos : 0,  time : 1000 } ],
           [ { todo : 'show', obj : W4,             time : 10000 } ]
         ],

   standbysun : [ [  { todo : 'move', obj : Hand, pos : 0, time : 1000 },
                     { todo : 'move', obj : Sun,  pos : 0, time : 1000 }
                ] ]
}

// *************************************************************
// logging
//   tell  -> log normal string
//   tello -> log any object (typical debug call)
// *************************************************************

var DEBUG = "Debug";
var ERROR = "Error";
var INFO  = "Info";

function tell(prefix, msg) {
   console.log(prefix + " : " + msg);
}

function tello(prefix, obj) {
   tell(prefix, JSON.stringify(obj));
}

// *************************************************************
// Constructor of page (body.onLoad)
// *************************************************************

var app = {
   // **********************************************************
   // Constructor of page (body.onLoad)
   // **********************************************************

   constructor : function () {
      tell(DEBUG, "Window size: [" + window.innerWidth + "," + window.innerHeight + "]");
      tell(DEBUG, "Page X,Y:    [" + window.pageXOffset + "," + window.pageYOffset + "]");
      tell(DEBUG, "Screen X,Y:  [" + window.screenX + "," + window.screenY + "]");
      tell(DEBUG, "Screen size: [" + screen.width + "," + screen.height + "]");
      tell(DEBUG, "Screen size: [" + screen.availWidth + "," + screen.availHeight + "]");

      var maxX = screen.availWidth;
      var maxY = screen.availHeight * 0.7;

      // Initialize hand object
      Hand.object             = document.getElementById("img_hand");
      // Start position
      Hand.positions[0].x     = maxX - Hand.object.width;
      Hand.positions[0].y     = (Hand.object.height - 120);
      // Position 1
      Hand.positions[1].x     = maxX * 0.3;
      Hand.positions[1].y     = maxY * 0.05;
      // Position 2
      Hand.positions[2].x     = Hand.positions[1].x + (maxX - Hand.positions[1].x) * 0.3;
      Hand.positions[2].y     = Hand.positions[1].y + (maxY - Hand.positions[1].y) * 0.3;
      // Set actual position
      Hand.actpos.x           = Hand.positions[0].x;
      Hand.actpos.y           = Hand.positions[0].y;
      Hand.object.style.left   = Hand.actpos.x + 'px';
      Hand.object.style.bottom = Hand.actpos.y + 'px';
      tello(DEBUG, Hand);

      var hand_center_x = Hand.object.width * 0.3;

      // Initialize sun object
      Sun.object              = document.getElementById("img_sun");
      // Start position
      Sun.positions[0].x      = 0;
      Sun.positions[0].y      = maxY * 0.4;
      // Position 1
      Sun.positions[1].x      = Hand.positions[1].x + hand_center_x - Sun.object.width / 2; 
      Sun.positions[1].y      = Hand.positions[1].y + Hand.object.height - Sun.object.height * 0.5;
      // Position 2
      Sun.positions[2].x      = Sun.positions[1].x + Hand.positions[2].x - Hand.positions[1].x;
      Sun.positions[2].y      = Sun.positions[1].y + Hand.positions[2].y - Hand.positions[1].y;
      // Position 3
      Sun.positions[3].x      = maxX - Sun.object.width - 10;
      Sun.positions[3].y      = maxY - Sun.object.height;
      // Set actual position
      Sun.actpos.x            = Sun.positions[0].x;
      Sun.actpos.y            = Sun.positions[0].y;
      Sun.object.style.left   = Sun.actpos.x + 'px';
      Sun.object.style.bottom = Sun.actpos.y + 'px';
      tello(DEBUG, Sun);

      // Initialize sentence 1 object
      W1.object               = document.getElementById("p_w1");
      tello(DEBUG, W1);

      // Initialize sentence 2 object
      W2.object               = document.getElementById("p_w2");
      tello(DEBUG, W2);

      // Initialize sentence 3 object
      W3.object               = document.getElementById("p_w3");
      tello(DEBUG, W3);

      // Initialize sentence 4 object
      W4.object               = document.getElementById("p_w4");
      tello(DEBUG, W4);
   },

   // **********************************************************
   // Move an object to a position
   // TODO: not just int straight line
   // **********************************************************

   move : function (obj, to_idx, lefttime, arg) {
      obj.object.style.visibility = "visible";

      if (obj.next_position != to_idx)
         obj.next_position= to_idx;

      if (lefttime === 0)
      {
         obj.actpos.x = obj.positions[to_idx].x;
         obj.actpos.y = obj.positions[to_idx].y;
         obj.object.style.left   = obj.actpos.x + 'px';
         obj.object.style.bottom = obj.actpos.y + 'px';

         if (obj.timer === null)
            clearTimeout(obj.timer);

         obj.timer = null;

         if (to_idx === 0)
            obj.object.style.visibility = "hidden";

         tello(DEBUG, "Move object ended");
         tello(DEBUG, arg);
         tello(DEBUG, obj);

         onMoveEnd(arg);

         return;
      }

      var range = {
         x : Math.abs(obj.actpos.x - obj.positions[to_idx].x),
         y : Math.abs(obj.actpos.y - obj.positions[to_idx].y)
      }

      if (range.x === 0 || range.y === 0)
      {
         tell(DEBUG, "Size of one range is zero [" + range.x + "," + range.y + "]");
         app.move(obj, to_idx, 0, arg);
         return;
      }

      var highest_range = ((range.x > range.y ) ? range.x : range.y);
      var sleep_tm = ((lefttime < 100) ? lefttime : lefttime / highest_range);
      var peace = lefttime/sleep_tm + ((lefttime % sleep_tm > 0) ? 1 : 0);
      var step = {
         x : range.x / peace,
         y : range.y / peace
      }

      if (step.x === 0 || step.y === 0)
      {
         tell(DEBUG, "Size of one step is zero [" + step.x + "," + step.y + "]");
         app.move(obj, to_idx, 0, arg);
         return;
      }

      obj.actpos.x = obj.actpos.x + ((obj.actpos.x > obj.positions[to_idx].x) ? -1 : 1) * step.x;
      obj.actpos.y = obj.actpos.y + ((obj.actpos.y > obj.positions[to_idx].y) ? -1 : 1) * step.y;
      obj.object.style.left   = obj.actpos.x + 'px';
      obj.object.style.bottom = obj.actpos.y + 'px';

      obj.timer = setTimeout(function () { app.move(obj, to_idx, lefttime - sleep_tm, arg); }, sleep_tm);

      tell(DEBUG, "Move object [" + (lefttime - sleep_tm) + "," + sleep_tm + "]");
   },

   // **********************************************************
   // run an animation
   // **********************************************************

   animate : function (id, cntr) {
      var animation = animations[id];

      tell(DEBUG, "1[" + id + "," + cntr + "]");

      if (animation === null)
      {
         tell(ERROR, "Animate abort, due to '" + id + "' is not known!");
         return ;
      }

      if (cntr === START ||  cntr === STOP)
      {
         var was_running = false;

         for (i = 0; i < animation.length; i++)
         {
            for (j= 0; j < animation[i].length; j++)
            {
               if (animation[i][j].obj === null)
               {
                  if (animation[i][j].timer !== null)
                  {
                      clearTimeout(animation[i][j].timer);
                      animation[i][j].timer = null;
                      was_running = true;
                  }
               }
               else if (animation[i][j].obj.timer !== null)
               {
                  clearTimeout(animation[i][j].obj.timer);
                  animation[i][j].obj.timer = null;
                  was_running = true;
               }
            }
         }

         if (was_running)
         {
            tell(INFO, "Animation '" + id + "' is killed!");
         }
      }

      if (cntr === STOP)
      {
         tell(INFO, "Animation '" + id + "' is stopped!");
         return;
      }

      if (cntr >= animation.length)
      {
         tell(INFO, "Animation '" + id + "' is ended [" + cntr + "," + animation.length + "]");
         return;
      }

      for (i= 0; i < animation[cntr].length; i++)
      {
         var step = animation[cntr][i];
//         tello(DEBUG, step);

         switch (step.todo)
         {
            case 'sleep' :
               tell(DEBUG, "Sleep");
               step.timer = setTimeout(function () { step.timer= null; app.animate(id, cntr + 1); }, step.sleep);

               break;

            case 'move'  :
               tell(DEBUG, "Move");
               app.move(step.obj, step.pos, step.time, { id : id, next : cntr + 1, i : cntr, j : i });

               break;

            case 'show'  :
               tell(DEBUG, "Show");
               step.obj.object.style.visibility = "visible";
               step.obj.object.timer = setTimeout(function () {
                  step.obj.object.timer= null;
                  step.obj.object.style.visibility = "hidden";
                  app.animate(id, cntr + 1);
               }, step.time);
               
               break;
         }
      }

      if (cntr === START)
      {
         tell(INFO, "Animation '" + id + "' is started!");
         return;
      }
   }
}

// *************************************************************
// Callback, when move is finished
// *************************************************************

function onMoveEnd(args)
{
   if (args.id === null)
   {
      tell(ERROR, "Internal error, due to args object of onMoveEnd is called without id!");
      return ;
   }
   if (animations[args.id] === null)
   {
      tell(ERROR, "Internal error, due to no animations with name '" + args.id + "'!");
      return ;
   }

   for (j= 0; j < animations[args.id][args.i].length; j++)
   {
      if (animations[args.id][args.i][j].obj.timer === null)
         continue;

      tell(DEBUG, "Animation has yet running substep!");
      return;
   }

   app.animate(args.id, args.next);
}

// *************************************************************
// GOOGLE world
// *************************************************************

function onSignIn(googleUser) {
    var profile = googleUser.getBasicProfile();
    console.log('ID: ' + profile.getId()); // Do not send to your backend! Use an ID token instead.
    console.log('Name: ' + profile.getName());
    console.log('Image URL: ' + profile.getImageUrl());
    console.log('Email: ' + profile.getEmail()); // This is null if the 'email' scope is not present.

    document.getElementById("h1_wtext").innerHTML = "Welcome " + profile.getName() + "!";

    app.animate("sun", START);
}

function signOut() {
   var auth2 = gapi.auth2.getAuthInstance();
   auth2.signOut().then(function () {
      console.log('User signed out.');

      document.getElementById("h1_wtext").innerHTML = "Welcome!"; 

      app.animate("sun",        STOP);
      app.animate("standbysun", START);
   });
}

