public enum OpCode
{
    CHAT_MESSAGE = 1,
    POSITION_MSG = 2,
    MAZE_GENERATION_MSG = 3,
    MOVE_MAZE_MSG = 4,   
    KILLENEMY_MSG = 5,
    CODE_MSG=6,
    OBJ_INNTERACTION_MSG=7,
    FOUND_RIDDLE_MSG = 8,
    RIDDLE_ANSWER_MSG = 9,

};
public enum objTypeCode
{
    PLAYER = 1,
    ENEMY = 2,
    OBJECT = 3,
}
public enum actionTypeCode
{
   RESTART=1,
   ROTATE=2
}
public enum pickUpObjCode
{
    TORCH=1,
    SWORD=2,
    MORNINGSTAR=3
}