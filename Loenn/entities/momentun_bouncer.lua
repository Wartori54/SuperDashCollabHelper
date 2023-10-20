local utils = require("utils")
local drawableSpriteStruct = require("structs.drawable_sprite")

local function getTexture(entity)
    return entity.texture or entity.texture ~= "default00"
end

local momentumBouncerVertical = {}
local momentumBouncerHorizontal = {}

local momentumBouncers = {
    momentumBouncerVertical,
    momentumBouncerHorizontal,
}

momentumBouncerVertical.name = "SuperDashCollabHelper/MomentumBouncerVertical"
momentumBouncerVertical.depth = -8500
momentumBouncerVertical.canResize = {false, true}
momentumBouncerVertical.texture = "objects/SuperDashCollabHelper/momentumBouncer/default"
function momentumBouncerVertical.selection(room, entity)
    return utils.rectangle(entity.x, entity.y, 8, entity.height)
end
function momentumBouncerVertical.sprite(room, entity)
    local textureRaw = getTexture(entity)
    local texture = "objects/SuperDashCollabHelper/momentumBouncer/" .. textureRaw
    local x, y = entity.x or 0, entity.y or 0
    local height = entity.height or 8
    local len = math.floor(height/8)-1
    local sprites = {}
    
    for i = 0, len do
        local regionX = 0
        local regionY = entity.flipped and 8 or 0
        
        if i == 0 then
            regionX = 0
        elseif i == len then
        -- todo: hecking fix longer textures (see cool00) to actually point to the last tile and not the 3rd one
            regionX = 16
        else
            regionX = 8
        end
        
        local sprite = drawableSpriteStruct.fromTexture(texture, entity)
        
        sprite:setJustification(0, 0)
        sprite:addPosition(8, i * 8)
        sprite:useRelativeQuad(regionX, regionY, 8, 8)
        sprite.rotation = math.pi / 2;
        table.insert(sprites, sprite)
    end
    
    return sprites
end
momentumBouncerVertical.placements = {
    {
        name = "leftwards",
        data = {
            flipped = true,
            height = 32,
            texture = "default00",
        }
    },
    {
        name = "rightwards",
        data = {
            flipped = false,
            height = 32,
            texture = "default00",
        }
    },
}

momentumBouncerHorizontal.name = "SuperDashCollabHelper/MomentumBouncerHorizontal"
momentumBouncerHorizontal.canResize = {true, false}
momentumBouncerHorizontal.texture = "objects/SuperDashCollabHelper/momentumBouncer/default"
function momentumBouncerHorizontal.selection(room, entity)
    return utils.rectangle(entity.x, entity.y, entity.width, 8)
end
function momentumBouncerHorizontal.sprite(room, entity)
    local textureRaw = getTexture(entity)
    local texture = "objects/SuperDashCollabHelper/momentumBouncer/" .. textureRaw
    local x, y = entity.x or 0, entity.y or 0
    local width = entity.width or 8
    local len = math.floor(width/8)-1
    local sprites = {}
    
    for i = 0, len do
        local regionX = 0
        local regionY = entity.flipped and 0 or 8
        
        if i == 0 then
            regionX = 0
        elseif i == len then
            -- todo: hecking fix longer textures (see cool00) to actually point to the last tile and not the 3rd one
            regionX = 16
        else
            regionX = 8
        end
        
        local sprite = drawableSpriteStruct.fromTexture(texture, entity)
        
        sprite:setJustification(0, 0)
        sprite:addPosition(i * 8, 0)
        sprite:useRelativeQuad(regionX, regionY, 8, 8)
        table.insert(sprites, sprite)
    end
    
    return sprites
end
momentumBouncerHorizontal.placements = {
    {
        name = "upwards",
        data = {
            flipped = true,
            width = 32,
            texture = "default00",
        }
    },
    {
        name = "downwards",
        data = {
            flipped = false,
            width = 32,
            texture = "default00",
        }
    },
}

return momentumBouncers
