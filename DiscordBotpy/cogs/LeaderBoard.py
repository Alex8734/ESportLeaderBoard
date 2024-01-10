import subprocess

import discord_slash.model
from discord.ext import commands
import json
import discord
import os
from discord_slash.utils.manage_components import create_button, create_actionrow
from discord_slash.model import ButtonStyle
import requests
from discord_slash import SlashCommand,SlashContext, cog_ext
from discord_slash.model import SlashCommandOptionType
from discord_slash.utils.manage_commands import create_choice,create_option

def GetGames() -> list:
    out = requests.get(f"http://localhost:5001/LeaderBoard/Types", headers={'Content-Type': 'application/json'},)
    if not out.ok:
        raise Exception(f"Error: {out.status_code}-{out.reason}")
    return list(map(lambda x: create_choice(name=x,value=x),out.json()))


class LeaderBoard(commands.Cog):
    def __init__(self, bot):
        self.bot = bot
        self.leaderboards = []

    @cog_ext.cog_slash(
        name='initplayers',
        description='Init User List on LeaderBoard API')

    @commands.has_permissions(administrator = True)
    async def InitPlayers(self, ctx : SlashContext, ip : str):
        members = list(filter(lambda m: not m.bot, ctx.guild.members))
        requestPlayers = list(map(lambda x: {"name":x.display_name,"profilePicture":str(x.avatar_url)},members))
        out = requests.put(f"http://localhost:5001/Player", json=requestPlayers,  headers={'Content-Type': 'application/json'}, )
        await ctx.send(f"```css\n{out.status_code}-{out.reason}```")

    @cog_ext.cog_slash(
        name='showleaderboard',
        description='Show LeaderBoard',
        options=[
            create_option(
                name="board",
                description="Witch LeaderBoard do you want to see?",
                option_type=SlashCommandOptionType.STRING,
                required=False,
                choices=GetGames()
            )
        ])
    @commands.has_permissions(administrator = True)
    async def ShowLeaderBoard(self, ctx : SlashContext, board : str):
        self.leaderboards.append(ctx.message)
        await ctx.send("Loading LeaderBoard...")


def setup(bot):
    bot.add_cog(LeaderBoard(bot))